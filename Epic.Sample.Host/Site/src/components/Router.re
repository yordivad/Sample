type state = {location: string};

type action =
  | NavigateTo(string);

let component = ReasonReact.reducerComponent("router");

let make = _children => {
  ...component,
  initialState: () => {location: ""},
  didMount: self => {
    let watcherID =
      ReasonReact.Router.watchUrl(url =>
        switch (url.path) {
        | [] => self.send(NavigateTo("home"))
        | _ => self.send(NavigateTo("not-found"))
        }
      );
    self.onUnmount(() => ReasonReact.Router.unwatchUrl(watcherID));
  },
  reducer: (action, state) =>
    switch (action) {
    | NavigateTo(value) => ReasonReact.Update({...state, location: value})
    },
  render: self =>
    switch (self.state.location) {
    | _ => <Home />
    },
};
