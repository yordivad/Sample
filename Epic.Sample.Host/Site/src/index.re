module App = {
  let component = ReasonReact.statelessComponent("Stadistic");
  let make = _children => {...component, render: _self => <Router />};
};
ReactDOMRe.renderToElementWithClassName(<App />, "app");
