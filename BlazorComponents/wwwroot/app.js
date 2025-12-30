// Maintain a set of subscribers
const tokenSubscribers = new Set();

export function subscribe(dotNetRef) {
    if (!dotNetRef) return;

    // Add to the subscriber set
    tokenSubscribers.add(dotNetRef);
}

export function notify() {
    // Notify all subscribers
    tokenSubscribers.forEach(dotNetRef => {
        try {
            dotNetRef.invokeMethodAsync("OnTokenChanged");
        } catch (e) {
            console.error("Failed to notify subscriber", e);
        }
    });
}

export function unsubscribe(dotNetRef) {
    tokenSubscribers.delete(dotNetRef);
}
