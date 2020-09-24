import React from 'react';
import {RootState} from "../../store/reducer";
import {Dispatch} from "redux";
import {connect, ConnectedProps} from "react-redux";
import {Container} from "@material-ui/core";
import "./Test.scss"

const mapStateToProps = (state: RootState) => ({})

const mapDispatchToProps = (dispatch: Dispatch) => ({})

const connector = connect(mapStateToProps, mapDispatchToProps);
type ReduxTypes = ConnectedProps<typeof connector>;


const Test = (props: ReduxTypes) => {


    return (
        <Container className={"Text"}>

        </Container>
    );

}


export default connector(Test);
