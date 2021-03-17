<?php
include 'vimeo.php';
	 set_time_limit(1200);

$vimeo = new phpVimeo('025a290d305f1dd097420908e19a175ca5d6013a', 'f21cd0620d43a7aac177a69447ef38a59828b6b4', 'dd28a7700044b3b939861f06fd4d891f', '52ae7957a2dab29bcc94d2503d44eeafadaa26bd');

     //   echo "Initializing video upload";
try {
	define('ROOTPATH', dirname(__FILE__));	
     $video = $_POST['video'];
	// $video="BodyPart_ca9beb6a-cac6-4914-8512-1e7603042056";
     $videoName = $_POST['tittle'];
	// $videoName="prueba";
	 //echo "empezando";
	//	echo ROOTPATH.'/../media/upload/VideosTemp/'.$video;

    $video_id = $vimeo->upload(ROOTPATH.'/../media/upload/VideosTemp/'.$video);

    if ($video_id) {
        echo 'http://player.vimeo.com/video/' . $video_id;

        //$vimeo->call('vimeo.videos.setPrivacy', array('privacy' => 'nobody', 'video_id' => $video_id));
        $vimeo->call('vimeo.videos.setTitle', array('title' => $videoName, 'video_id' => $video_id));
        $vimeo->call('vimeo.videos.setDescription', array('description' => '', 'video_id' => $video_id));
    }
    else {
        echo "Video file did not exist!";
    }
}
catch (VimeoAPIException $e) {
    echo "Encountered an API error -- code {$e->getCode()} - {$e->getMessage()}";
	echo ROOTPATH.'/../media/upload/VideosTemp/'.$video;
}