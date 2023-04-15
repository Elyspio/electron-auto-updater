import { Container, Paper, Stack } from "@mui/material";
import "./Apps.scss";
import React, { useEffect } from "react";
import { App } from "./App";
import { useAppSelector } from "../../../store";
import { getApps } from "../../../store/module/app/apps.async.actions";
import { useActions } from "../../hooks/useActions";

const Apps = () => {
	const apps = useAppSelector(s => Object.keys(s.apps.all));

	const actions = useActions({ getApps });

	useEffect(() => {
		console.count("Apps");
		actions.getApps();
	}, [actions]);

	return (
		<Container className={"Apps"}>
			<Paper>
				<Stack p={2} spacing={2}>
					{apps.map(app => (
						<App key={app} name={app} />
					))}
				</Stack>
			</Paper>
		</Container>
	);
};

export default Apps;
