import React from "react";
import { AppArch, AppVersion } from "../../../core/apis/backend/generated";
import { Grid, Typography } from "@mui/material";

type AppDownloadProgress = {
	name: string;
	arch: AppArch;
	version: AppVersion;
	state: "waiting" | "downloading" | "downloaded";
};

export const AppDownloadProgress = ({ arch, name, version, state }: AppDownloadProgress) => {
	return (
		<Grid container direction={"column"}>
			<Grid item>
				<Typography>
					{name} {arch} {version.raw}
				</Typography>
			</Grid>
			<Grid item>
				<Typography>
					{state === "waiting" && "Waiting for server"}
					{state === "downloading" && "Downloading"}
					{state === "downloaded" && "Downloaded"}
				</Typography>
			</Grid>
		</Grid>
	);
};