<?php
include 'vimeo.php';

$vimeo = new phpVimeo('025a290d305f1dd097420908e19a175ca5d6013a', 'f21cd0620d43a7aac177a69447ef38a59828b6b4', 'dd28a7700044b3b939861f06fd4d891f', '52ae7957a2dab29bcc94d2503d44eeafadaa26bd');

try {
	  $video_id = $_POST['videoid'];
     set_time_limit(300);
         $response=$vimeo->call('vimeo.videos.getThumbnailUrls', array('video_id' => $video_id));
		echo $response->thumbnails->thumbnail[0]->_content;
}
catch (VimeoAPIException $e) {
    echo "Encountered an API error -- code {$e->getCode()} - {$e->getMessage()}";
}
