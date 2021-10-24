import React from "react";
import ReactDOM from "react-dom";
import "./index.scss";
import { Provider } from "react-redux";
import store, { useAppSelector } from "./store";
import Application from "./view/components/Application";
import { ThemeProvider } from "@material-ui/core";
import { themes } from "./config/theme";
import { Config } from "./config/window";

declare global {
	interface Window {
		config: Config;
	}
}

function Wrapper() {
	const theme = useAppSelector(state => (state.theme.current === "dark" ? themes.dark : themes.light));

	return (
		<ThemeProvider theme={theme}>
			<Application />
		</ThemeProvider>
	);
}

function App() {
	return (
		<Provider store={store}>
			<Wrapper />
		</Provider>
	);
}

ReactDOM.render(<App />, document.getElementById("root"));

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
