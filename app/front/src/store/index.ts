import { configureStore } from "@reduxjs/toolkit";
import { TypedUseSelectorHook, useDispatch, useSelector } from "react-redux";
import { themeReducer } from "./module/theme/theme.reducer";
import { Container } from "inversify";
import { container } from "../core/di";
import { appsReducer } from "./module/app/apps.reducer";

const store = configureStore({
	reducer: {
		theme: themeReducer,
		apps: appsReducer,
	},
	devTools: process.env.NODE_ENV !== "production",
	middleware: getDefaultMiddleware => getDefaultMiddleware({ thunk: { extraArgument: { container: container } } }),
});

export type StoreState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<StoreState> = useSelector;

export default store;

export type ExtraArgument = { container: Container };
