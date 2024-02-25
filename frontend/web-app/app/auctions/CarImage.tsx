'use client';

import Image from "next/image";
import React from "react";

type Props = {
  imageUrl: string;

}

export default function CarImage({imageUrl}: Props) {
  const [isLoading, setIsLoading] = React.useState(true);

  return (
      <Image
          src={imageUrl}
          alt="image of a car"
          fill={true}
          priority={true}
          className={`group-hover:opacity-75 duration-700 ease-in-out object-cover
          ${isLoading ? 'grayscale blur-2xl scale-110' : 'grayscale-0 blur-0 scale-100'}`}
          sizes="(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 25vw"
          onLoad={() => setIsLoading(false)} // This is the new way of doing it
          // onLoadingComplete={() => setIsLoading(false)} // This is the old way of doing it
      />
  );
}