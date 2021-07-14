import * as React from "react";

export const useAsyncEffect = (func: () => Promise<any>, dependencies: any[]) => {
    React.useEffect(() => {
        (async () => {
            await func();
        })()
    }, dependencies)
}
