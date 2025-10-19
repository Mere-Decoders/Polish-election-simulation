import Listener from './Listener.js';

export default class Observer {
	constructor() {
		if (Observer.instance) {
			return Observer.instance;
		}
		
		console.log("I've been created");

		this.listeners = [];
		this.eventNames = [];
		Observer.instance = this;
	}

	addListener(listener) {
		if (!(listener instanceof Listener)) {
			throw new Error("`listener` must extend the Listener class");
		}

		this.listeners.push(listener);
	}

	addEventType(eventName) {
		if (!this.eventNames.includes(eventName)) {
			this.eventNames.push(eventName);
			console.log("Observer: new event type: \"" + eventName + "\"");
		}
	}

	removeListener(listener) {
		this.listeners = this.listeners.filter(element => element !== listener);
	}

	sendEvent(event) {

		if (!("name" in event))
			throw new Error("There is no \"name\" field in the \"event\" actuall argument of \"sendEvent\"");

		if (!("payload" in event))
			throw new Error("There is no \"payload\" field in the \"event\" actuall argument of \"sendEvent\"");

		if (!this.eventNames.includes(event.name))
			throw new Error("There is no \"" + event.name + "\" event type on the list. It might be a typo, or you have forgotten to add this event type using \"addEventType(eventName)\"");


		console.log("Observer: Emitting \"" + event.name + "\" event");

		for (const listener of this.listeners) {
			listener.handleEvent(event);
		}
	}
}
