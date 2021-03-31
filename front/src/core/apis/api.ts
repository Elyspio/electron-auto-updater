type apiCallResponse<T> = { data: T, status: number };
export type apiCall<T = any> = <T>(url: string, data?: object) => Promise<apiCallResponse<T>>;

export class Api {

    protected readonly endpoint: string

    protected constructor(endpoint: string) {
        this.endpoint = endpoint;
        console.debug("Create new API with endpoint : ", endpoint)
    }

    public static redirect = (newLocation) => {
        window.location.href = newLocation
    }

    public get: apiCall = async <T>(url, params) => {
        const qs = params ? Object.keys(params).map(key => key + '=' + params[key]).join('&') : "";
        const res = await fetch(`${this.endpoint}${url}?${qs}`, {method: "GET"})
        return await this.handleRequest<T>(res);

    }

    public post: apiCall = async <T>(url, params) => {
        const res = await fetch(`${this.endpoint}${url}`, {method: "POST", body: JSON.stringify(params)})
        return await this.handleRequest<T>(res);
    }

    public put: apiCall = async <T>(url, params) => {
        const res = await fetch(`${this.endpoint}${url}`, {method: "PUT", body: JSON.stringify(params)})
        return await this.handleRequest<T>(res);

    }

    public delete: apiCall = async <T>(url, params) => {
        const res = await fetch(`${this.endpoint}${url}`, {method: "DELETE", body: JSON.stringify(params)})
        return await this.handleRequest<T>(res);
    }

    private async handleRequest<T = any>(res: Response): Promise<apiCallResponse<T>> {
        const txt = await res.text();

        let data;
        let parsed = false
        try {
            data = JSON.parse(txt)
            parsed = true
        } catch (e) {
        }

        return {
            data: (parsed ? data : txt) as T,
            status: res.status
        }
    }

}

