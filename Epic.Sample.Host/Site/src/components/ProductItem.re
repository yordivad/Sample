open Css;

let highPrice = style([backgroundColor(red)]);
let normalPrice = style([backgroundColor(white)]);

type product = {
  name: string,
  price: float,
  quantity: float,
};

let componet = ReasonReact.statelessComponent("product");

let make = (~item: product, _children) => {
  ...componet,
  render: _self => {
    let color = item.price > 3.0 ? highPrice : normalPrice;

    <tr className=color>
      <td> {ReasonReact.string(item.name)} </td>
      <td> {ReasonReact.string(string_of_float(item.quantity))} </td>
      <td> {ReasonReact.string("$" ++ string_of_float(item.price))} </td>
    </tr>;
  },
};
