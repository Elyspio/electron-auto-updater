import { useMemo } from "react";
import { useAppDispatch } from "../../store";
import { ActionCreatorsMapObject, bindActionCreators } from "redux";

export function useActions<T extends ActionCreatorsMapObject>(actions: T) {
	const dispatch = useAppDispatch();

	// eslint-disable-next-line react-hooks/exhaustive-deps
	return useMemo(() => bindActionCreators(actions, dispatch), [dispatch]);
}
