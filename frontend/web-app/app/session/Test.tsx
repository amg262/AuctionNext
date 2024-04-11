'use client'

import React, {useState} from 'react'
import {Button} from 'flowbite-react';
import {getPosts} from "@/app/actions/auctionActions";


export default function Test() {
  const [loading, setLoading] = useState(false);
  const [result, setResult] = useState<any>();

  function doUpdate() {
    console.log('Test')
    setResult(undefined);
    setLoading(true);
    getPosts()
        .then(res => setResult(res))
        .finally(() => setLoading(false))

    // fetch('https://api.auctionnext.com/post')
    //     .then(res => res.json())
    //     .then(res => {
    //       setResult(res)
    //       setLoading(false)
    //     }).catch(err => {
    //   console.log(err)
    //   setLoading(false)
    // });

  }

  return (
      <div className='flex items-center gap-4'>
        <Button outline isProcessing={loading} onClick={doUpdate}>
          Test
        </Button>
        <div>
          {JSON.stringify(result, null, 2)}
        </div>
      </div>
  )
}
