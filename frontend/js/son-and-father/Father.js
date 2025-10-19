import Observer from '../observer/Observer.js'

export default class Father {
	yell() {
		let observer = new Observer();
		observer.sendEvent({name: "son command", payload: "say thank you"});
	}

}
