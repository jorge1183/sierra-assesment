const { useHistory } = require("react-router-dom");

const useAjaxService = () => {
    const history = useHistory();

    const _fetch = async (url: string, includeToken: boolean = false, method: string = "GET", payload: any = null) => {
        const headers = new Headers({ 'Content-Type': 'application/json' });

        if (includeToken) {
            const token = sessionStorage.getItem("token");
            if (!token) {
                history.push("/login");
                throw Error("Not logged in");
            }
            headers.append('Authorization', `Bearer ${token}`);
        }
        const requestInit: RequestInit = {
            method: method,
            headers: headers,
        };

        if (payload) {
            requestInit.body = JSON.stringify(payload);
        }

        let fetchResult;

        try {
            fetchResult = await fetch(
                url,
                requestInit);
        } catch (error) {
            console.error("Fetch error", error);
            throw Error("Unexpected error");
        }

        if (!fetchResult.ok) {
            if (fetchResult.status === 401) {
                history.push("/login");
                throw Error("Not logged in");
            }
            const message = await fetchResult.text();
            console.error("Fetch error", message);
            throw Error("Unexpected error");
        }
        return await fetchResult.json();
    }

    const ajaxGet = async (url: string, includeToken: boolean = false): Promise<any> => await _fetch(url, includeToken);
    const ajaxPost = async (url: string, payload: any, includeToken: boolean) => await _fetch(url, includeToken, "POST", payload);
    const ajaxPut = async (url: string, id: number, payload: any, includeToken: boolean) => await _fetch(`${url}/${id}`, includeToken, "PUT", payload);

    return {
        ajaxGet,
        ajaxPost,
        ajaxPut,
    }
}

export default useAjaxService;