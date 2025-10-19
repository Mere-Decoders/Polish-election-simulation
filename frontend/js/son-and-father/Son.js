import Listener from '../observer/Listener.js'
import Observer from '../observer/Observer.js'

export default class Son extends Listener {
	constructor(name) {
		super()
		let observer = new Observer();
		observer.addListener(this);
		observer.addEventType("son command");

		this.name = name
	}

	handleEvent(event) {
		if (event.name == "son command") {
			console.log("I'm the " + this.name + " son and I've been told to " + event.payload);
		}
	}
}
