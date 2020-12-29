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
  $content = json_decode(file_get_contents("php://input"),true);

  $db->registerUser(
    $content['username'],
    $content['mail'],
    password_hash($content['password'], PASSWORD_DEFAULT));

});



//Prevent acces from file name
$router->post('/login', function($request, $db) {
  //getting query content
  $content = json_decode(file_get_contents("php://input"),true);

  $user = $db->connectUser(
    $content['uid'],
    $content['password']
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