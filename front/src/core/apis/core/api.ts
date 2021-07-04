/* tslint:disable */
/* eslint-disable */
/**
 * Api documentation
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: 1.0.0
 *
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */


import {Configuration} from './configuration';
import globalAxios, {AxiosInstance, AxiosPromise} from 'axios';
// Some imports not used depending on template conditions
// @ts-ignore
import {
    assertParamExists,
    createRequestFunction,
    DUMMY_BASE_URL,
    serializeDataIfNeeded,
    setApiKeyToObject,
    setBasicAuthToObject,
    setBearerAuthToObject,
    setOAuthToObject,
    setSearchParams,
    toPathString
} from './common';
// @ts-ignore
import {BASE_PATH, BaseAPI, COLLECTION_FORMATS, RequestArgs, RequiredError} from './base';

/**
 *
 * @export
 * @interface Forbidden
 */
export interface Forbidden {
    /**
     * The error name
     * @type {string}
     * @memberof Forbidden
     */
    name: string;
    /**
     * An error message
     * @type {string}
     * @memberof Forbidden
     */
    message: string;
    /**
     * The status code of the exception
     * @type {number}
     * @memberof Forbidden
     */
    status: number;
    /**
     * A list of related errors
     * @type {Array<GenericError>}
     * @memberof Forbidden
     */
    errors?: Array<GenericError>;
    /**
     * The stack trace (only in development mode)
     * @type {Array<string>}
     * @memberof Forbidden
     */
    stack?: Array<string>;
}

/**
 *
 * @export
 * @interface GenericError
 */
export interface GenericError {
    /**
     * The error name
     * @type {string}
     * @memberof GenericError
     */
    name: string;
    /**
     * An error message
     * @type {string}
     * @memberof GenericError
     */
    message: string;

    [key: string]: object | any;
}

/**
 * ExampleApi - axios parameter creator
 * @export
 */
export const ExampleApiAxiosParamCreator = function (configuration?: Configuration) {
    return {
        /**
         *
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        exampleGet: async (options: any = {}): Promise<RequestArgs> => {
            const localVarPath = `/core/test`;
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, DUMMY_BASE_URL);
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }

            const localVarRequestOptions = {method: 'GET', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;


            setSearchParams(localVarUrlObj, localVarQueryParameter, options.query);
            let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
            localVarRequestOptions.headers = {...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers};

            return {
                url: toPathString(localVarUrlObj),
                options: localVarRequestOptions,
            };
        },
        /**
         *
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        exampleGetAdmin: async (options: any = {}): Promise<RequestArgs> => {
            const localVarPath = `/core/test/admin`;
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, DUMMY_BASE_URL);
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }

            const localVarRequestOptions = {method: 'GET', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;


            setSearchParams(localVarUrlObj, localVarQueryParameter, options.query);
            let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
            localVarRequestOptions.headers = {...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers};

            return {
                url: toPathString(localVarUrlObj),
                options: localVarRequestOptions,
            };
        },
    }
};

/**
 * ExampleApi - functional programming interface
 * @export
 */
export const ExampleApiFp = function (configuration?: Configuration) {
    const localVarAxiosParamCreator = ExampleApiAxiosParamCreator(configuration)
    return {
        /**
         *
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async exampleGet(options?: any): Promise<(axios?: AxiosInstance, basePath?: string) => AxiosPromise<string>> {
            const localVarAxiosArgs = await localVarAxiosParamCreator.exampleGet(options);
            return createRequestFunction(localVarAxiosArgs, globalAxios, BASE_PATH, configuration);
        },
        /**
         *
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async exampleGetAdmin(options?: any): Promise<(axios?: AxiosInstance, basePath?: string) => AxiosPromise<string>> {
            const localVarAxiosArgs = await localVarAxiosParamCreator.exampleGetAdmin(options);
            return createRequestFunction(localVarAxiosArgs, globalAxios, BASE_PATH, configuration);
        },
    }
};

/**
 * ExampleApi - factory interface
 * @export
 */
export const ExampleApiFactory = function (configuration?: Configuration, basePath?: string, axios?: AxiosInstance) {
    const localVarFp = ExampleApiFp(configuration)
    return {
        /**
         *
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        exampleGet(options?: any): AxiosPromise<string> {
            return localVarFp.exampleGet(options).then((request) => request(axios, basePath));
        },
        /**
         *
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        exampleGetAdmin(options?: any): AxiosPromise<string> {
            return localVarFp.exampleGetAdmin(options).then((request) => request(axios, basePath));
        },
    };
};

/**
 * ExampleApi - object-oriented interface
 * @export
 * @class ExampleApi
 * @extends {BaseAPI}
 */
export class ExampleApi extends BaseAPI {
    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof ExampleApi
     */
    public exampleGet(options?: any) {
        return ExampleApiFp(this.configuration).exampleGet(options).then((request) => request(this.axios, this.basePath));
    }

    /**
     *
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof ExampleApi
     */
    public exampleGetAdmin(options?: any) {
        return ExampleApiFp(this.configuration).exampleGetAdmin(options).then((request) => request(this.axios, this.basePath));
    }
}


