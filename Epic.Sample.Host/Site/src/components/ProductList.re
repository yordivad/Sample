type state = {
  orderBy: string,
  products: option(array(ProductItem.product)),
};

type actions =
  | UpdateProducts(array(ProductItem.product));

let component = ReasonReact.reducerComponent("table");

let jsonToProduct = (json: Js.Json.t): ProductItem.product =>
  Json.Decode.{
    name: json |> field("name", string),
    price: json |> field("price", float),
    quantity: json |> field("quantity", float),
  };

let jsonToProducts = json => Json.Decode.array(jsonToProduct, json);

let fetchProducts = () =>
  Js.Promise.(
    Axios.get("/api/product/query?Query=query{top_products(first:5){name,quantity,price}}")
    |> then_(r => resolve(jsonToProducts(r##data##data##top_products)))
  );

let fetchOrderProducts = order =>
  Js.Promise.(
    Axios.get(
      "/api/product/query?Query=query{top_order_products(first:5,orderBy:\""
      ++ order
      ++ "\"){name,quantity,price}}",
    )
    |> then_(r => resolve(jsonToProducts(r##data##data##top_order_products)))
  );

let sortBy = (orderBy, {ReasonReact.state, ReasonReact.send}) =>
  fetchOrderProducts(orderBy)
  |> Js.Promise.then_(r => {
       send(UpdateProducts(r));
       Js.Promise.resolve();
     })
  |> ignore;

let make = _children => {
  ...component,

  initialState: () => {products: None, orderBy: ""},

  didMount: self =>
    fetchProducts()
    |> Js.Promise.then_(r => {
         self.send(UpdateProducts(r));
         Js.Promise.resolve();
       })
    |> ignore,

  reducer: (action, state) =>
    switch (action) {
    | UpdateProducts(value) => ReasonReact.Update({...state, products: Some(value)})
    },

  render: self => {
    let renderProducts =
      switch (self.state.products) {
      | Some(products) =>
        ReasonReact.array(Array.map((p: ProductItem.product) => <ProductItem key={p.name} item=p />, products))
      | None => ReasonReact.string("not loaded topics")
      };
    <table>
      <thead>
        <tr>
          <th onClick={_ => sortBy("name", self)}> {ReasonReact.string("Product")} </th>
          <th onClick={_ => sortBy("quantity", self)}> {ReasonReact.string("Quantity")} </th>
          <th onClick={_ => sortBy("price", self)}> {ReasonReact.string("Price")} </th>
        </tr>
      </thead>
      <tbody> renderProducts </tbody>
    </table>;
  },
};
