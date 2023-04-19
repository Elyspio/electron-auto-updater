import React, { useCallback, useMemo } from "react";
import { Box, Chip, Stack, Typography } from "@mui/material";
import { AppArch } from "../../../core/apis/backend/generated";
import ArrowForwardIosSharpIcon from "@mui/icons-material/ArrowForwardIosSharp";
import MuiAccordion, { AccordionProps } from "@mui/material/Accordion";
import MuiAccordionSummary, { AccordionSummaryProps } from "@mui/material/AccordionSummary";
import MuiAccordionDetails from "@mui/material/AccordionDetails";
import { styled } from "@mui/styles";
import { AppVersionComponent } from "./AppVersion";
import Divider from "@mui/material/Divider";
import { useAppSelector } from "../../../store";

const Accordion = styled((props: AccordionProps) => <MuiAccordion disableGutters elevation={0} square {...props} />)(({ theme }) => ({
	border: `1px solid ${theme.palette.divider}`,
	"&:not(:last-child)": {
		borderBottom: 0,
	},
	"&:before": {
		display: "none",
	},
}));

const AccordionSummary = styled((props: AccordionSummaryProps) => <MuiAccordionSummary expandIcon={<ArrowForwardIosSharpIcon sx={{ fontSize: "0.9rem" }} />} {...props} />)(
	({ theme }) => ({
		backgroundColor: theme.palette.mode === "dark" ? "rgba(255, 255, 255, .05)" : "rgba(0, 0, 0, .03)",
		flexDirection: "row-reverse",
		"& .MuiAccordionSummary-expandIconWrapper.Mui-expanded": {
			transform: "rotate(90deg)",
		},
		"& .MuiAccordionSummary-content": {
			marginLeft: theme.spacing(1),
		},
	})
);

const AccordionDetails = styled(MuiAccordionDetails)(({ theme }) => ({
	padding: theme.spacing(2),
	borderTop: "1px solid rgba(0, 0, 0, .125)",
}));

type AppProps = { name: string };

function ExpandMoreIcon() {
	return null;
}

export function App({ name }: AppProps) {
	const archWithVersions = useAppSelector(s => s.apps.all[name]);

	const getColor = useCallback((arch: string) => {
		return [AppArch.Win32, AppArch.Win64].includes(arch as any) ? "primary" : "success";
	}, []);

	const versionsNode = useMemo(
		() =>
			Object.entries(archWithVersions)
				.filter(([_, versions]) => versions.length)
				.map(([arch, versions], i, arr) => (
					<>
						<Stack direction={"row"} spacing={3} alignItems={"center"} key={arch}>
							<Box minWidth={"5rem"}>
								<Chip label={arch} color={getColor(arch)} variant={"outlined"} />
							</Box>
							<Stack direction={"row"} flexWrap={"wrap"} maxWidth={"100%"}>
								{versions.map((version, versionI, versionArr) => (
									<AppVersionComponent latest={versionI === versionArr.length - 1} key={version.raw} name={name} arch={arch as AppArch} version={version} />
								))}
							</Stack>
						</Stack>
						{i < arr.length - 1 && <Divider flexItem key={`divider-${arch}`} />}
					</>
				)),
		[getColor, name, archWithVersions]
	);

	return (
		<Accordion>
			<AccordionSummary expandIcon={<ExpandMoreIcon />}>
				<Typography variant={"overline"} fontSize={16}>
					{name}
				</Typography>
			</AccordionSummary>
			<AccordionDetails>
				<Stack spacing={2}>{versionsNode}</Stack>
			</AccordionDetails>
		</Accordion>
	);
}
