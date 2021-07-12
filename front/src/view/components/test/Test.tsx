import {CircularProgress, Container} from "@material-ui/core";
import "./Test.scss"
import Typography from "@material-ui/core/Typography";
import Button from '@material-ui/core/Button';
import {Services} from "../../../core/services";
import * as React from 'react';
import {useAsyncEffect} from "../../hooks/useAsyncEffect";
import {useAsyncCallback} from "../../hooks/useAsyncCallback";

const Test = () => {


	const [msg, setMsg] = React.useState("");
	const [admin, setAdmin] = React.useState("");

	useAsyncEffect(async () => {
		const {data} = await Services.example.getContent();
		setMsg(data)
	}, [])

	const [fetchAdmin, {isExecuting}] = useAsyncCallback(async () => {
		const data = await Services.example.getAdminContent();
		if (data) {
			setAdmin(data)
		}
	}, [])

	return (
		<Container className={"Test"}>
			<Typography variant={"h6"} color={"textPrimary"}>Test</Typography>
			<Typography color={"textPrimary"}>msg: {msg}</Typography>

			<Button onClick={fetchAdmin}>{isExecuting ? <CircularProgress/> : "Admin content"}</Button>
			{admin && <Typography color={"textPrimary"}>admin response: {admin}</Typography>}

		</Container>
	);

}


export default Test;
