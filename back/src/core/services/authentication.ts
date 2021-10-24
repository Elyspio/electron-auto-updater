import { Apis } from "../apis";
import { Log } from "../utils/decorators/logger";
import { getLogger } from "../utils/logger";

export class AuthenticationService {
	private static log = getLogger.service(AuthenticationService);

	@Log(AuthenticationService.log)
	public async isAuthenticated(token: string) {
		const result = await Apis.authentication.validToken(token);
		AuthenticationService.log.info("AuthenticationService.isAuthenticated", {
			data: result.data,
			status: result.status,
		});
		return result.status === 204;
	}
}
