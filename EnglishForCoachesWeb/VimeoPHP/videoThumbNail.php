
<?php
/*ini_set('display_errors', 1);
ini_set('display_startup_errors', 1);
error_reporting(E_ALL);*/
try {
	  $video_id = $_POST['videoid'];
	//  $video_id="https://player.vimeo.com/video/224491543";
     set_time_limit(300);
	
	     if( !$video_id ) return false;
	
$referer = 'http://englishforcoaches.com';

/*function _isCurl(){
    return function_exists('curl_version');
}
echo _isCurl();*/
	$curl = curl_init();
curl_setopt($curl, CURLOPT_URL, 'http://vimeo.com/api/oembed.json?url=' . $video_id );
curl_setopt($curl, CURLOPT_USERAGENT, 'Mozilla/5.0 (X11; U; Linux i686; pl-PL; rv:1.9.0.2) Gecko/2008092313 Ubuntu/9.25 (jaunty) Firefox/3.8');
curl_setopt($curl, CURLOPT_HTTPHEADER, $header);
curl_setopt($curl, CURLOPT_HEADER, 0);
curl_setopt($curl, CURLOPT_REFERER, $referer);
curl_setopt($curl, CURLOPT_ENCODING, 'gzip,deflate');
curl_setopt($curl, CURLOPT_AUTOREFERER, true);
curl_setopt($curl, CURLOPT_RETURNTRANSFER, true);
curl_setopt($curl, CURLOPT_FOLLOWLOCATION, true);
curl_setopt($curl, CURLOPT_TIMEOUT, 300);

$data = json_decode(curl_exec($curl));
//echo curl_getinfo($ch) . '<br/>';
//echo curl_errno($ch) . '<br/>';
//echo curl_error($ch) . '<br/>';



		
		//echo var_dump($data);
    if( !$data ) return false;
		echo $data->thumbnail_url;
}
catch (VimeoAPIException $e) {
    echo "Encountered an API error -- code {$e->getCode()} - {$e->getMessage()}";
}
