export default function Details({params}: { params: { id: string } }) {
  return (
      <div>Details for {params.id}</div>
  );
}