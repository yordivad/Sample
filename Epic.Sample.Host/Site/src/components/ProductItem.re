type product = {
  name: string,
  price: float,
  quantity: float,
};

let componet = ReasonReact.statelessComponent("product");

let make = (~item: product, _children) => {
  ...componet,
  render: _self =>
    <tr>
      <td> {ReasonReact.string(item.name)} </td>
      <td> {ReasonReact.string(string_of_float(item.price))} </td>
      <td> {ReasonReact.string(string_of_float(item.quantity))} </td>
    </tr>,
};
