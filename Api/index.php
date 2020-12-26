<?php
//Include classes
include_once $_SERVER['DOCUMENT_ROOT'].'/objects/Database.php';
include_once $_SERVER['DOCUMENT_ROOT'].'/objects/Request.php';
include_once $_SERVER['DOCUMENT_ROOT'].'/objects/Router.php';
include_once $_SERVER['DOCUMENT_ROOT'].'/objects/Token.php';


//Include Environnement file
include_once $_SERVER['DOCUMENT_ROOT'].'/env.php';

//Instanciate objects
$db = new Database();
$router = new Router(new Request, $db);

//Prevent acces from file name
$router->get('/', function() {
  include_once $_SERVER['DOCUMENT_ROOT'].'/pages/portal.php';
  die();
});

$router->post('/register', function($request, $db) {
  $db->registerUser(
    $request->getBody()['username'],
    $request->getBody()['mail'],
    password_hash($request->getBody()['password'], PASSWORD_DEFAULT));

});



//Prevent acces from file name
$router->post('/login', function($request, $db) {
  $body = $request->getBody();

  $user = $db->connectUser(
    $body['uid'],
    $body['password']
  );
  if($user != null)
  {
    http_response_code(200);
    echo Token::generate($user["username"]);
  }else{
    http_response_code(400);
  }
  die();
});



?>