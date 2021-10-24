import { IMiddleware, Middleware } from "@tsed/common";
import { NotFound } from "@tsed/exceptions";

@Middleware()
export class RequireLogin implements IMiddleware {
	public use() {
		if (process.env.NODE_ENV === "production") {
			throw new NotFound("This resource is not accessible in production");
		}
	}
}
