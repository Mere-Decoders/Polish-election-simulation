import Observer from '../observer/Observer.js'

const form = document.querySelector("#voting_options_form");
const observer = new Observer();

async function handleForm() {
	const formData = new FormData(form);

	console.log(formData);

	observer.sendEvent({name:"voting options change", payload: formData});
}

observer.addEventType("voting options change");

form.addEventListener("change", (event) => {
	event.preventDefault();
	handleForm();
});
