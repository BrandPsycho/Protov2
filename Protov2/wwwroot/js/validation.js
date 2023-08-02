// validation.js
const nombreInput = document.getElementById('nombre_cliente');
const apellidoInput = document.getElementById('apellido_cliente');
const correoInput = document.getElementById('correo');
const correoError = document.getElementById('correo-error');
const telefonoInput = document.getElementById('telefono_cliente');
const telefonoError = document.getElementById('telefono-error');

telefonoInput.addEventListener('input', validateTelefono);


nombreInput.addEventListener('keydown', blockNumbers);
apellidoInput.addEventListener('keydown', blockNumbers);
correoInput.addEventListener('input', validateCorreo);

function blockNumbers(event) {
  const key = event.key;
  if (/[0-9]/.test(key)) {
    event.preventDefault();
  }
}


function validateTelefono() {
  const telefono = telefonoInput.value;
  const telefonoPattern = /^09[0-9]{0,8}$/;

  if (telefonoPattern.test(telefono) && telefono.length === 10) {
    telefonoError.textContent = '';
  } else if (telefono !== '' && !telefonoPattern.test(telefono.slice(0, 2))) {
    telefonoError.textContent = 'El teléfono debe comenzar con "09"';
  } else if (telefono === '') {
    telefonoError.textContent = '';
  } else {
    telefonoError.textContent = 'El teléfono debe tener 10 dígitos.';
  }
}

function checkTelefono() {
  const telefono = telefonoInput.value;

  if (telefono === '') {
    telefonoError.textContent = '';
  } else if (telefono.length > 10) {
    telefonoInput.value = telefono.slice(0, 10);
  }
}

function isNumberKey(event) {
    const charCode = event.which ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        event.preventDefault();
        return false;
    }
    return true;
}


// VALIDACION DE CORREO
function validateCorreo() {
  const correo = correoInput.value;
  const correoPattern = /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;

  if (correoPattern.test(correo)) {
    correoError.textContent = '';
  } else if (correo === '') {
    correoError.textContent = '';
  } else {
    correoError.textContent = 'Formato correcto: usuario@dominio.com';
  }
}
