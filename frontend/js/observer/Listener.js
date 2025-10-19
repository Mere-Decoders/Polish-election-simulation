export default class Listener {
	constructor() {
		if (new.target === Listener)
			throw new Error("Listener is an abstract class, you can not instantiate it.");
	}

	handleEvent(event) {
		throw new Error("`handleEvent(event)` should be implemented by the child Class.");
	}
}
