import * as React from "react";
import "./Application.scss";
import Brightness5Icon from "@material-ui/icons/Brightness5";
import Brightness3Icon from "@material-ui/icons/Brightness3";
import { useAppDispatch, useAppSelector } from "../../store";
import { toggleTheme } from "../../store/module/theme/action";
import { createDrawerAction, withDrawer } from "./utils/drawer/Drawer.hoc";
import { Box } from "@material-ui/core";
import AppStorage from "./appStorage/AppStorage";

function Application() {
	const dispatch = useAppDispatch();

	const { theme, icon } = useAppSelector(s => ({
		theme: s.theme.current,
		icon: s.theme.current === "dark" ? <Brightness5Icon /> : <Brightness3Icon />,
	}));

	const drawer = withDrawer({
		component: <AppStorage />,
		actions: [
			createDrawerAction(theme === "dark" ? "Light Mode" : "Dark Mode", {
				icon,
				onClick: () => dispatch(toggleTheme()),
			}),
		],
		title: "Apps updater",
	});

	return (
		<Box className={"Application"} bgcolor={"background.default"}>
			{drawer}
		</Box>
	);
}

export default Application;
