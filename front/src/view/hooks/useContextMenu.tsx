import React from "react";

type Delta = {
	top: number;
	left: number;
};

export function useContextMenu({ top, left }: Delta) {
	const [contextMenu, setContextMenu] = React.useState<{
		mouseX: number;
		mouseY: number;
	} | null>(null);

	const handleContextMenu = (event: React.MouseEvent) => {
		event.preventDefault();
		setContextMenu({
			mouseX: event.clientX - left,
			mouseY: event.clientY - top,
		});
	};

	const handleClose = React.useCallback(() => {
		setContextMenu(null);
	}, []);

	const position = React.useMemo(
		() =>
			contextMenu !== null
				? {
						top: contextMenu.mouseY,
						left: contextMenu.mouseX,
				  }
				: undefined,
		[contextMenu]
	);
	return {
		onContextMenu: handleContextMenu,
		close: handleClose,
		position,
		open: React.useMemo(() => contextMenu !== null, [contextMenu]),
	};
}