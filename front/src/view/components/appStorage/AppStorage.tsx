import React from 'react';
import {RootState} from "../../store/reducer";
import {Dispatch} from "redux";
import {connect, ConnectedProps} from "react-redux";
import {Container} from "@material-ui/core";
import "./AppStorage.scss"
import Typography from "@material-ui/core/Typography";
import {Services} from "../../../core/services";

const mapStateToProps = (state: RootState) => ({})

const mapDispatchToProps = (dispatch: Dispatch) => ({})

const connector = connect(mapStateToProps, mapDispatchToProps);
type ReduxTypes = ConnectedProps<typeof connector>;


const AppStorage = (props: ReduxTypes) => {


    const [apps, setApps] = React.useState<string[]>([]);

    React.useEffect(() => {
        (async () => {
            setApps((await Services.core.listApps()).data)
        })();
    }, [])


    return (
        <Container className={"AppStorage"}>
            <Typography>Apps: {apps.join(", ")}</Typography>
        </Container>
    );

}


export default connector(AppStorage);
