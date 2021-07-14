import React from 'react';
import {Container} from "@material-ui/core";
import "./AppStorage.scss"
import Typography from "@material-ui/core/Typography";
import {Services} from "../../../core/services";


const AppStorage = () => {


	const [apps, setApps] = React.useState<string[]>([]);

	React.useEffect(() => {
		(async () => {
			setApps((await Services.appStorage.listApps()).data)
		})();
	}, [])


	return (
		<Container className={"AppStorage"}>
			<Typography color={"textPrimary"}>Apps: {apps.join(", ")}</Typography>
		</Container>
	);

}


export default (AppStorage);
