import { createSlice } from "@reduxjs/toolkit";
import { deleteApp, getApps } from "./apps.async.actions";
import { AppArch, AppMetadata } from "../../../core/apis/backend/generated";
import { PartialRecord } from "../../utils/utils.types";
import semver from "semver/preload";

export interface AppsState {
	all: Record<AppMetadata["name"], PartialRecord<AppMetadata["arch"], AppMetadata["version"][]>>;
}

const defaultState: AppsState = {
	all: {},
};

export const slice = createSlice({
	name: "apps",
	initialState: defaultState,
	reducers: {},
	extraReducers: builder => {
		builder.addCase(getApps.fulfilled, (state, action) => {
			const metadatas = action.payload;
			state.all = {};

			for (let metadata of metadatas) {
				state.all[metadata.name] ??= {};
				state.all[metadata.name] = Object.keys(metadata.archs).reduce((acc, current) => {
					acc[current] = metadata.archs[current];
					return acc;
				}, {} as PartialRecord<AppMetadata["arch"], AppMetadata["version"][]>);
				for (let arch of Object.keys(state.all[metadata.name])) {
					state.all[metadata.name][arch as AppArch]!.sort((v1, v2) => semver.compare(v1.raw, v2.raw));
				}
			}
		});
		builder.addCase(deleteApp.fulfilled, (state, action) => {
			let arg = action.meta.arg;
			state.all[arg.name][arg.arch] = state.all[arg.name][arg.arch]!.filter(version => version.raw !== arg.version.raw);
		});
	},
});

export const { reducer: appsReducer } = slice;
