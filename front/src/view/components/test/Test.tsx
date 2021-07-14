import {Container} from "@material-ui/core";
import "./Test.scss"
import Typography from "@material-ui/core/Typography";
import Button from '@material-ui/core/Button';
import {Services} from "../../../core/services";
import * as React from 'react';

const Test = () => {


    const [msg, setMsg] = React.useState("");
    const [admin, setAdmin] = React.useState("");

    React.useEffect(() => {
        const fetchData = async () => {
            const {data} = await Services.example.getContent();
            setMsg(data)
        }
        fetchData();
    }, [])


    const fetchAdmin = async () => {
        const data = await Services.example.getAdminContent();
        if (data) {
            setAdmin(data)
        }
    }

    return (
        <Container className={"Test"}>
            <Typography variant={"h6"}>Test</Typography>
            <Typography>msg: {msg}</Typography>

            <Button onClick={fetchAdmin}>Admin content</Button>
            {admin && <Typography>admin response: {admin}</Typography>}

        </Container>
    );

}


export default Test;
