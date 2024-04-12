/** @type {import('next').NextConfig} */
const nextConfig = {
  experimental: {
    serverActions: true,
  },
  images: {
    domains: [
      'cdn.pixabay.com',
      'source.unsplash.com',
      'images.unsplash.com',
    ],
  },
  output: 'standalone',
};

module.exports = nextConfig;
