import { Box, Container, Grid, Paper } from "@mui/material";
import "./Apps.scss";
import * as React from "react";
import { useInjection } from "inversify-react";
import { AppsService } from "../../../core/services/apps.service";
import { DiKeysService } from "../../../core/di/services/di.keys.service";
import { App } from "./App";
import { useAsyncEffect } from "../../hooks/useAsyncEffect";

const Apps = () => {
	const services = {
		apps: useInjection<AppsService>(DiKeysService.apps),
	};

	const [apps, setApps] = React.useState<string[]>([]);

	useAsyncEffect(async () => {
		setApps(await services.apps.getAppsName());
	}, [services.apps]);

	return (
		<Container className={"Apps"}>
			<Paper>
				<Box p={2}>
					<Grid container direction={"column"} spacing={2}>
						{apps.map(app => (
							<Grid item key={app}>
								<App name={app} />
							</Grid>
						))}
					</Grid>
				</Box>
			</Paper>
		</Container>
	);
};

export default Apps;
