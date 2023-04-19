import React, { useCallback } from "react";
import { AppArch, AppVersion } from "../../../core/apis/backend/generated";
import { Box, Button, Menu, MenuItem } from "@mui/material";
import { useContextMenu } from "../../hooks/useContextMenu";
import { deleteApp, downloadApp } from "../../../store/module/app/apps.async.actions";
import { useActions } from "../../hooks/useActions";

type AppVersionProps = {
	version: AppVersion;
	name: string;
	arch: AppArch;
	latest: boolean;
};

export function AppVersionComponent({ name, version, arch, latest }: AppVersionProps) {
	const actions = useActions({ deleteApp, downloadApp });

	const { open, onContextMenu, close, position } = useContextMenu({ top: 40, left: 0 });

	const download = useCallback(() => {
		actions.downloadApp({ name: name, arch, version });
	}, [actions, arch, name, version]);

	const deleteDb = useCallback(() => {
		actions.deleteApp({ name, arch, version });
		close();
	}, [actions, arch, close, name, version]);

	return (
		<Box key={version.raw} px={1}>
			<Button variant={latest ? "outlined" : "text"} color={latest ? "secondary" : "inherit"} onContextMenu={onContextMenu} onClick={download}>
				{version.raw}
			</Button>
			<Menu open={open} onClose={close} anchorReference="anchorPosition" anchorPosition={position}>
				<MenuItem color={"error"} onClick={deleteDb}>
					Delete
				</MenuItem>
			</Menu>
		</Box>
	);
}
