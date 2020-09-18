import React from 'react';
import {RootState} from "../../store/reducer";
import {Dispatch} from "redux";
import {connect, ConnectedProps} from "react-redux";
import {Container} from "@material-ui/core";
import "./Test.scss"
import {TestApi} from "../../api/test";
import {Api} from "../../api/api";
import Typography from "@material-ui/core/Typography";

const mapStateToProps = (state: RootState) => ({})

const mapDispatchToProps = (dispatch: Dispatch) => ({})

const connector = connect(mapStateToProps, mapDispatchToProps);
type ReduxTypes = ConnectedProps<typeof connector>;


const Test = (props: ReduxTypes) => {


    const [msg, setMsg] = React.useState("");
    const [msgAdmin, setMsgAdmin] = React.useState("");

    React.useEffect(() => {

        const fetchData = async () => {
            const api = TestApi.instance;
            setMsg((await api.getContent()).data)

            // admin
            const admin = await api.getAdminContent();
            if(!admin) {
                Api.redirectToLoginPage();
            }
            else {
                setMsgAdmin(admin.data)
            }


        }
        fetchData();


    }, [])


    return (
        <Container className={"Text"}>
            <Typography>msg: {msg}</Typography>
            <Typography>admin msg: {msgAdmin}</Typography>
        </Container>
    );

}


export default connector(Test);
