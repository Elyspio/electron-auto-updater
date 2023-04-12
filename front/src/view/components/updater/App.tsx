import React, {useCallback} from "react";
import {Box, Button, Chip, Grid, Menu, MenuItem, Stack, Typography} from "@mui/material";
import {useInjection} from "inversify-react";
import {AppsService} from "../../../core/services/apps.service";
import {DiKeysService} from "../../../core/di/services/di.keys.service";
import {useAsyncState} from "../../hooks/useAsyncState";
import {AppArch} from "../../../core/apis/backend/generated";
import {useContextMenu} from "../../hooks/useContextMenu";
import ArrowForwardIosSharpIcon from "@mui/icons-material/ArrowForwardIosSharp";
import MuiAccordion, {AccordionProps} from "@mui/material/Accordion";
import MuiAccordionSummary, {AccordionSummaryProps} from "@mui/material/AccordionSummary";
import MuiAccordionDetails from "@mui/material/AccordionDetails";
import {styled} from "@mui/styles";

const Accordion = styled((props: AccordionProps) => <MuiAccordion
    disableGutters elevation={0}
    square {...props} />)(({theme}) => ({
    border: `1px solid ${theme.palette.divider}`,
    "&:not(:last-child)": {
        borderBottom: 0,
    },
    "&:before": {
        display: "none",
    },
}));

const AccordionSummary = styled((props: AccordionSummaryProps) => <MuiAccordionSummary
    expandIcon={<ArrowForwardIosSharpIcon sx={{fontSize: "0.9rem"}}/>} {...props} />)(
    ({theme}) => ({
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

const AccordionDetails = styled(MuiAccordionDetails)(({theme}) => ({
    padding: theme.spacing(2),
    borderTop: "1px solid rgba(0, 0, 0, .125)",
}));


type AppProps = { name: string };

function ExpandMoreIcon() {
    return null;
}

export function App({name}: AppProps) {
    const services = {
        apps: useInjection<AppsService>(DiKeysService.apps),
    };

    const fetchVersions = useCallback(() => services.apps.getAppVersions(name), [services.apps, name]);

    const {data: versions} = useAsyncState(fetchVersions, {
        Win32: [],
        Win64: [],
        LinuxDeb: [],
        LinuxSnap: [],
        LinuxRpm: [],
    });

    const getColor = useCallback((arch: string) => {
        return [AppArch.Win32, AppArch.Win64].includes(arch as any) ? "primary" : "success";
    }, []);

    const download = useCallback(
        (version, arch) => () => {
            services.apps.download(name, arch, version);
        },
        [services, name]
    );

    const {open, onContextMenu, close, position} = useContextMenu({top: 40, left: 0});

    return (
        <Accordion>
            <AccordionSummary expandIcon={<ExpandMoreIcon/>}>
                <Typography variant={"overline"} fontSize={16}>
                    {name}
                </Typography>
            </AccordionSummary>
            <AccordionDetails>
                <Stack spacing={2}>
                    {Object.entries(versions)
                        .filter(([_, versions]) => versions.length)
                        .map(([arch, versions]) => (
                            <Stack direction={"row"} spacing={3} alignItems={"center"}>
                                <Box minWidth={"5rem"}>
                                    <Chip label={arch} color={getColor(arch)} variant={"outlined"}/>
                                </Box>
                                <Stack direction={"row"} spacing={2} flexWrap={"wrap"} maxWidth={"100%"}>
                                    {versions.map(version => (
                                        <Grid item key={version.raw}>
                                            <Button onContextMenu={onContextMenu} color={"inherit"} variant={"text"}
                                                    onClick={download(version, arch)}>
                                                {version.raw}
                                            </Button>
                                            <Menu elevation={1} open={open} onClose={close}
                                                  anchorReference="anchorPosition" anchorPosition={position}>
                                                <MenuItem onClick={close}>Delete</MenuItem>
                                            </Menu>
                                        </Grid>
                                    ))}
                                </Stack>
                            </Stack>
                        ))}
                </Stack>
            </AccordionDetails>
        </Accordion>


    );
}