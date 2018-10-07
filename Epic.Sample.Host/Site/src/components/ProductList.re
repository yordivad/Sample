type state = {products: option(array(ProductItem.product))};

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
    Axios.get("http://localhost:5000/api/product/query?Query=query{top_products(first:5){name,quantity,price}}")
    |> then_(r => resolve(jsonToProducts(r##data##data##top_products)))
  );

let make = _children => {
  ...component,

  initialState: () => {products: None},

  didMount: self =>
    fetchProducts()
    |> Js.Promise.then_(r => {
         self.send(UpdateProducts(r));
         Js.Promise.resolve();
       })
    |> ignore,

  reducer: (action, _state) =>
    switch (action) {
    | UpdateProducts(value) => ReasonReact.Update({products: Some(value)})
    },

  render: self => {
    let renderProducts =
      switch (self.state.products) {
      | Some(products) =>
        ReasonReact.array(Array.map((p: ProductItem.product) => <ProductItem key={p.name} item=p />, products))
      | None => ReasonReact.string("not loaded topics")
      };
    <table>
      <tr>
        <th> {ReasonReact.string("Product")} </th>
        <th> {ReasonReact.string("Quantity")} </th>
        <th> {ReasonReact.string("Price")} </th>
      </tr>
      renderProducts
    </table>;
  },
};
