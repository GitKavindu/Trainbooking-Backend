export function startResizeListener(dotNetRef) {

    const notify = () => {
        dotNetRef.invokeMethodAsync("OnScreenWidthChanged", window.innerWidth);
    };

    window.addEventListener("resize", notify);

    // initial value
    notify();
}
