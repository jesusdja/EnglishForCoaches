
<?php
ini_set('display_errors', 'On');
error_reporting(E_ALL);
// Load the autoloader
if (file_exists('vimeo3/vendor/autoload.php')) {
	// Composer
	require_once('vimeo3/vendor/autoload.php');
} else {
	// Custom
	require_once(  'vimeo3/vendor/autoload.php');
}


use Vimeo\Vimeo;
use Vimeo\Exceptions\VimeoUploadException;
/**
 *   Copyright 2013 Vimeo
 *
 *   Licensed under the Apache License, Version 2.0 (the "License");
 *   you may not use this file except in compliance with the License.
 *   You may obtain a copy of the License at
 *
 *       http://www.apache.org/licenses/LICENSE-2.0
 *
 *   Unless required by applicable law or agreed to in writing, software
 *   distributed under the License is distributed on an "AS IS" BASIS,
 *   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *   See the License for the specific language governing permissions and
 *   limitations under the License.
 */
/*$config = require(__DIR__ . '/init.php');
if (empty($config['access_token'])) {
    throw new Exception(
        'You can not upload a file without an access token. You can find this token on your app page, or generate ' .
        'one using `auth.php`.'
    );
}*/

// Instantiate the library with your client id, secret and access token (pulled from dev site)
$lib = new Vimeo("025a290d305f1dd097420908e19a175ca5d6013a", "f21cd0620d43a7aac177a69447ef38a59828b6b4", "94aab7229a7c0898f6d54d2692e0bb02");

// Create a variable with a hard coded path to your file system
	define('ROOTPATH', dirname(__FILE__));	
     $video = $_POST['video'];
$file_name = ROOTPATH.'/../media/upload/VideosTemp/'.$video;
$videoName = $_POST['tittle'];
//$file_name = ROOTPATH.'/../media/upload/VideosTemp/119.mp4';
//$videoName = 'test';
//echo 'Uploading: ' . $file_name . "\n";
try {
set_time_limit(0);
//ini_set('memory_limit', '2048M'); 
    // Upload the file and include the video title and description.
    $uri = $lib->upload($file_name, array(
        'name' => $videoName,
        'description' => ""
    ));
    // Get the metadata response from the upload and log out the Vimeo.com url
    $video_data = $lib->request($uri . '?fields=link');
   // echo '"' . $file_name . ' has been uploaded to ' . $video_data['body']['link'] . "\n";
    // Make an API call to edit the title and description of the video.
    $lib->request($uri, array(
        'name' => $videoName,
        'description' => "",
    ), 'PATCH');
 //   echo 'The title and description for ' . $uri . ' has been edited.' . "\n";
    // Make an API call to see if the video is finished transcoding.
    $video_data = $lib->request($uri . '?fields=transcode.status');
   // echo 'The transcode status for ' . $uri . ' is: ' . $video_data['body']['transcode']['status'] . "\n";
	//echo 'http://player.vimeo.com/video/' . $uri;
	echo 'http://player.vimeo.com/video/' . str_replace( "/videos/", "",$uri);
} catch (VimeoUploadException $e) {
    // We may have had an error. We can't resolve it here necessarily, so report it to the user.
    echo 'Error uploading ' . $file_name . "\n";
    echo 'Server reported: ' . $e->getMessage() . "\n";
} catch (VimeoRequestException $e) {
    echo 'There was an error making the request.' . "\n";
    echo 'Server reported: ' . $e->getMessage() . "\n";
}