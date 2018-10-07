module App = {
  let component = ReasonReact.statelessComponent("sample");
  let make = _children => {...component, render: _self => <ProductList />};
};
ReactDOMRe.renderToElementWithClassName(<App />, "app");
