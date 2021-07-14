import * as React from 'react';
import {Paper} from "@material-ui/core";
import "./Application.scss"
import Brightness5Icon from '@material-ui/icons/Brightness5';
import Brightness3Icon from '@material-ui/icons/Brightness3';
import Example from "./test/Test";
import {useAppDispatch, useAppSelector} from "../../store";
import {toggleTheme} from "../../store/module/theme/action";
import {withDrawer} from "./utils/drawer/Drawer.hoc";

function Application() {

    const dispatch = useAppDispatch();

    const icon = useAppSelector(s => s.theme.current === "dark" ? <Brightness5Icon/> : <Brightness3Icon/>)

    const drawer = withDrawer({
        component: <Example/>,
        actions: [{
            component: {
                icon,
                onClick: () => dispatch(toggleTheme()),
            },
            description: {children: "Switch Lights"}
        }],
        title: "Example"
    })


    return (
        <Paper square={true} className={"Application"}>
            {drawer}
        </Paper>
    );
}


export default Application
