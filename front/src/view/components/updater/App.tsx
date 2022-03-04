import { Button, Chip, Grid, Typography } from "@mui/material";
import { useInjection } from "inversify-react";
import { AppsService } from "../../../core/services/apps.service";
import { DiKeysService } from "../../../core/di/services/di.keys.service";
import { useCallback } from "react";
import { useAsyncState } from "../../hooks/useAsyncState";
import { AppArch } from "../../../core/apis/backend/generated";

type AppProps = { name: string };

export function App({ name }: AppProps) {
	const services = {
		apps: useInjection<AppsService>(DiKeysService.apps),
	};

	const fetchVersions = useCallback(() => services.apps.getAppVersions(name), [services.apps, name]);

	const { data: versions } = useAsyncState(fetchVersions, {
		Win32: [],
		Win64: [],
		LinuxDeb: [],
		LinuxSnap: [],
		LinuxRpm: [],
	});

	const getColor = useCallback((arch: string) => {
		return [AppArch.Win32, AppArch.Win64].includes(arch as any) ? "primary" : "success";
	}, []);


	const download = useCallback((version, arch) => () => {
		services.apps.download(name, arch, version)
	}, [services, name])

	return (
		<Grid container className="App" direction={"column"}>
			<Grid container item justifyContent={"center"} my={2}>
				<Grid item>
					<Typography variant={"overline"} fontSize={16}>
						{name}
					</Typography>
				</Grid>
			</Grid>

			<Grid spacing={2} item container flexDirection={"column"} margin={0} width={"100%"}>
				{Object.entries(versions).map(([arch, versions]) => (
					<Grid item container key={arch} bgcolor={"background.default"} p={2} my={1}>
						<Grid item container minWidth={100} xs={1}>
							<Chip label={arch} color={getColor(arch)} variant={"outlined"} />
						</Grid>

						<Grid item container spacing={2} xs>
							{versions.map(version => (
								<Grid item key={version.raw}>
									<Button color={"inherit"} variant={"text"} onClick={download(version, arch)}>
										{version.raw}
									</Button>
								</Grid>
							))}
						</Grid>
					</Grid>
				))}
			</Grid>
		</Grid>
	);
}