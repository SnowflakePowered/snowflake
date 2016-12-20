import * as React from "react";
import * as ReactDOM from "react-dom";
import { AppContainer } from "react-hot-loader";
import App from "./App";

const rootElement = document.getElementById("root");
ReactDOM.render(
    <AppContainer>
        <App />
    </AppContainer>,
    rootElement
);

declare var module: any;
declare var require: any;
// Hot Module Replacement API
if (module.hot) {
  module.hot.accept("./App", () => {
    const NextApp = require("./App").default;
    ReactDOM.render(
      <AppContainer>
        <NextApp/>
      </AppContainer>
      ,
      rootElement
    );
  });
}