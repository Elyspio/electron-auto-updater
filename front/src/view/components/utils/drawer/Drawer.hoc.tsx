import React from "react";
import {ActionComponent, ActionComponentProps, ActionDescription, ActionDescriptionProps} from "./actions/Action";
import {Box, Paper, Typography} from "@material-ui/core";
import {Drawer} from "./Drawer";
import "./actions/Actions.scss"

type WithDrawerProps = {
    component: React.ReactNode,
    actions: {
        component: ActionComponentProps,
        description: ActionDescriptionProps
    }[],
    title: string

}

function Actions(props: { elements: WithDrawerProps["actions"] }) {
    return <Box className={"Actions"}>
        {props.elements.map(action => <ActionComponent  {...action.component}>
            <ActionDescription children={action.description.children}/>
        </ActionComponent>)}
    </Box>;
}


export function withDrawer({component, title, actions}: WithDrawerProps) {

    return <Box className={"Drawer-hoc"}>
        <Paper className={"header"}>
            <Typography variant={"h4"} align={"center"}>{title}</Typography>
        </Paper>
        <Drawer position={"right"} actionsComponent={<Actions elements={actions}/>}>
            <div className="content">
                {component}
            </div>
        </Drawer>
    </Box>

}
