import React from "react";
import { createAsyncActionGenerator, getService } from "../../utils/utils.actions";
import { AppMetadata } from "../../../core/apis/backend/generated";
import { AppsService } from "../../../core/services/apps.service";
import { toast } from "react-toastify";
import { Typography } from "@mui/material";

const createAsyncThunk = createAsyncActionGenerator("apps");

export const downloadApp = createAsyncThunk("download", ({ version, arch, name }: AppMetadata, { extra }) => {
	const appService = getService(AppsService, extra);

	return appService.download(name, arch, version);
});

export const deleteApp = createAsyncThunk("delete", async ({ version, arch, name }: AppMetadata, { extra }) => {
	const appService = getService(AppsService, extra);
	await toast.promise(appService.delete(name, arch, version), {
		error: "An error occured during download",
		success: {
			render: () => (
				<Typography>
					{name} | {arch} | {version.raw} is now deleted
				</Typography>
			),
		},
		pending: {
			render: () => (
				<Typography>
					Deleting {name} | {arch} | {version.raw}
				</Typography>
			),
		},
	});
});

export const getApps = createAsyncThunk("get", async (_, { extra }) => {
	const appService = getService(AppsService, extra);

	const names = await appService.getAppsName();

	return (
		await Promise.all(
			names.map(async name => ({
				name,
				archs: await appService.getAppVersions(name),
			}))
		)
	).flat();
});
