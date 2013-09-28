static var triggered=0;

function OnTriggerEnter  (other : Collider) {
triggered=1;
}

function OnTriggerExit  (other : Collider) {
triggered=0;
}